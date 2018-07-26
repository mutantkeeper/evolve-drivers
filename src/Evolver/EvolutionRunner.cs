using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evolver
{
    class EvolutionRunner
    {
        public delegate void UpdateEventHandler(object sender, EventArgs e);
        public event UpdateEventHandler UpdateEvent;
        public Map Map { get; private set; }
        public Car[] Cars { get; private set; }
        public HashSet<Position> Collisions { get; private set; }

        private bool isStopped;
        private readonly Random random = new Random();
        private int[] mutants;
        private int numMutants;
        private WorldDbContext dbContext;
        private MutantManager mutantManager;
        private EvolverSettings settings;
        private SortedDictionary<long, HashSet<int>> mutantsByScore;
        private int turnsNoSucceed = 0;
        private int turnsNoReplicate = 0;
        private sbyte[][] mutantMemory;


        public EvolutionRunner(WorldDbContext dbContext, MutantManager mutantManager, EvolverSettings settings, int[] mutants = null)
        {
            this.dbContext = dbContext;
            this.mutantManager = mutantManager;
            this.settings = settings;
            this.mutants = mutants;
            numMutants = mutants == null ? dbContext.NumMutants : mutants.Length;
            mutantMemory = new sbyte[numMutants][];

            SetUpMap();

            mutantsByScore = new SortedDictionary<long, HashSet<int>>();
            for (int i = 0; i < numMutants; ++i)
            {
                var mutantStat = dbContext.Mutants.Find(GetMutantId(i));
                AddMutantScore(i, mutantStat.Score);
            }
        }

        public int GetMutantId(int i)
        {
            return mutants == null ? i : mutants[i];
        }

        private void SetUpMap()
        {
            Cars = new Car[numMutants];
            Collisions = new HashSet<Position>();
            Map = new Map((sbyte)settings.mapSize, (sbyte)settings.mapSize);

            for (int i = 0; i < numMutants; ++i)
            {
                Cars[i] = new Car();
                SetRandomCarDirection(i);
                Cars[i].Position = GetRandomBlankPosition();
                if (!settings.driveThru)
                    Map[Cars[i].Position] = Cars[i].Direction;
            }
            for (int i = 0; i < numMutants; ++i)
            {
                Cars[i].Goal = GetRandomBlankPosition();
            }
            for (int i = 0; i < numMutants; ++i)
                mutantMemory[i] = new sbyte[256];
        }

        private void SetRandomCarDirection(int i)
        {
            byte flags = (byte)(random.Next(4) << 2);
            Cars[i].Direction = PaneState.Moving | (PaneState)flags;
        }

        public void Run()
        {
            NativeMethods.PreventSystemSleep();

            isStopped = false;
            dbContext.Configuration.AutoDetectChangesEnabled = false;
            dbContext.MapResets = 0;
            int consecutiveStaleTurns = 0;
            do
            {
                Collisions.Clear();
                Position?[] newPositions = new Position?[numMutants];
                Parallel.For(0, numMutants,
                    new ParallelOptions { MaxDegreeOfParallelism = settings.numParallelThreads == 0 ? -1 : settings.numParallelThreads },
                    (i, state) =>
                    {
                        var mutant = mutantManager.GetMutant(i);
                        var car = Cars[i];
                        var decision = mutant.MakeDecision(Map, car, mutantMemory[i]);
                        switch (decision)
                        {
                            case Decision.Move:
                                newPositions[i] = car.NextPosition();
                                break;
                            case Decision.TurnBack:
                                car.TurnBack();
                                break;
                            case Decision.TurnLeft:
                                car.TurnLeft();
                                break;
                            case Decision.TurnRight:
                                car.TurnRight();
                                break;
                            case Decision.Stay:
                                break;
                        }
                    });

                Dictionary<Position, List<int>> positionBookings = new Dictionary<Position, List<int>>();
                for (int i = 0; i < numMutants; ++i)
                {
                    if (!newPositions[i].HasValue)
                        continue;
                    var pos = newPositions[i].Value;
                    if (Collisions.Contains(pos))
                    {
                        continue;
                    }
                    if (Map[pos] != PaneState.Blank)
                    {
                        Collisions.Add(pos);
                        continue;
                    }
                    try
                    {
                        positionBookings.Add(pos, new List<int>());
                        positionBookings[pos].Add(i);
                    }
                    catch (ArgumentException)
                    {
                        if (settings.driveThru)
                        {
                            positionBookings[pos].Add(i);
                        }
                        else
                        {
                            Collisions.Add(pos);
                            positionBookings.Remove(pos);
                        }
                    }
                }

                if (positionBookings.Count <= numMutants / 20)
                {
                    ++consecutiveStaleTurns;
                }
                else
                {
                    consecutiveStaleTurns = 0;
                }

                int[] scoreChanges = new int[numMutants];

                ++turnsNoSucceed;
                foreach (var pair in positionBookings)
                {
                    foreach (var i in pair.Value)
                    {
                        var car = Cars[i];
                        if (settings.driveThru)
                        {
                            car.Position = pair.Key;
                        }
                        else
                        {
                            Map[car.Position] = PaneState.Blank;
                            car.Position = pair.Key;
                            Map[car.Position] = car.Direction;
                        }
                        int score = car.IsDirectionRight() ? 8 : -8;
                        if (car.Goal.Equals(car.Position))
                        {
                            score += 32;
                            car.Goal = GetRandomBlankPosition();
                            consecutiveStaleTurns = 0;
                            turnsNoSucceed = 0;
                        }
                        scoreChanges[i] = score;
                    }
                }

                if (positionBookings.Count > 0)
                {
                    for (int i = 0; i < numMutants; ++i)
                    {
                        var mutantStat = dbContext.Mutants.Find(GetMutantId(i));
                        mutantStat.Turns += 1;
                        UpdateMutantScore(i, mutantStat, scoreChanges[i]);
                    }
                }

                int totalDuplicates = 0;
                int totalMutatedBytes = 0;

                bool resetMap = false;
                bool recreateMutants = false;
                ++turnsNoReplicate;
                for (; ; )
                {
                    var oldHighScore = mutantsByScore.Last().Key;
                    var oldLowScore = mutantsByScore.First().Key;
                    if (oldLowScore + settings.eliminateThreshold > oldHighScore)
                    {
                        if (totalDuplicates > 0)
                            break;
                        if (turnsNoSucceed < (Map.Width + Map.Height) * 2)
                            break;
                        if (turnsNoReplicate < Map.Width * Map.Height)
                        {
                            resetMap = consecutiveStaleTurns >= (Map.Width + Map.Height) / 2;
                            break;
                        }
                    }
                    else
                    {
                        consecutiveStaleTurns = 0;
                    }
                    turnsNoReplicate = 0;
                    var winners = mutantsByScore.Last().Value;
                    var losers = mutantsByScore.First().Value;
                    var numReplicates = Math.Min(winners.Count, losers.Count);
                    var replicated = winners.Take(numReplicates).ToList();
                    var eliminated = losers.Take(numReplicates).ToList();
                    recreateMutants = (oldHighScore - oldLowScore) < settings.eliminateThreshold / 2;
                    if (oldHighScore == oldLowScore)
                    {
                        int[] sorted = new int[numMutants];
                        for (int i = 0; i < numMutants; ++i)
                        {
                            sorted[i] = i;
                        }
                        numReplicates = numMutants / 2;
                        eliminated = sorted.Take(numReplicates).ToList();
                        replicated = sorted.Skip(numMutants - numReplicates).ToList();
                    }
                    else if (oldLowScore + settings.eliminateThreshold > oldHighScore)
                    {
                        int[] sorted = new int[numMutants];
                        int i = 0;
                        foreach (var item in mutantsByScore)
                        {
                            foreach (var id in item.Value)
                                sorted[i++] = id;
                        }
                        numReplicates = numMutants / 2;
                        eliminated = sorted.Take(numReplicates).ToList();
                        replicated = sorted.Skip(numMutants - numReplicates).ToList();
                    }

                    int[] mutatedBytes = new int[replicated.Count];
                    Parallel.For(0, replicated.Count,
                        new ParallelOptions { MaxDegreeOfParallelism = settings.numParallelThreads == 0 ? -1 : settings.numParallelThreads },
                        (i, state) =>
                        {
                            var winner = GetMutantId(replicated[i]);
                            var loser = GetMutantId(eliminated[i]);
                            if (recreateMutants)
                            {
                                mutantManager.Recreate(loser);
                                mutatedBytes[i] += dbContext.MutantSize;
                            }
                            else
                            {
                                mutatedBytes[i] += mutantManager.Replicate(winner, loser, settings.mostBytesToMutate);
                            }
                            mutantMemory[eliminated[i]] = new sbyte[256];
                        });
                    totalMutatedBytes += mutatedBytes.Sum();

                    for (int i = 0; i < replicated.Count; ++i)
                    {
                        var winner = replicated[i];
                        var loser = eliminated[i];

                        var winnerStat = dbContext.Mutants.Find(GetMutantId(winner));
                        var loserStat = dbContext.Mutants.Find(GetMutantId(loser));

                        // loserStat.Turns = 0;
                        loserStat.Generation = winnerStat.Generation + 1;

                        var scoreTransfer = (oldHighScore - oldLowScore) / 4;
                        UpdateMutantScore(winner, winnerStat, -scoreTransfer);
                        UpdateMutantScore(loser, loserStat, scoreTransfer);

                        Map[Cars[loser].Position] = PaneState.Blank;
                        Cars[loser].Position = GetRandomBlankPosition();
                        SetRandomCarDirection(loser);
                        if (!settings.driveThru)
                            Map[Cars[loser].Position] = Cars[loser].Direction;
                        SetRandomGoal(loser);
                    }
                    totalDuplicates += replicated.Count;
                }

                ++dbContext.TotalTurns;
                dbContext.TotalReplicates += totalDuplicates;
                dbContext.TotalMutatedBytes += totalMutatedBytes;

                UpdateEvent?.Invoke(this, new EventArgs());
                Application.DoEvents();

                if (resetMap)
                {
                    SetUpMap();
                    consecutiveStaleTurns = 0;
                    ++dbContext.MapResets;
                }
            } while (!isStopped);

            mutantManager.Flush();
            dbContext.Configuration.AutoDetectChangesEnabled = true;
            dbContext.SaveChanges();

            NativeMethods.AllowSystemSleep();
        }

        private void UpdateMutantScore(int mutant, MutantStat mutantStat, long change)
        {
            if (change != 0)
            {
                var score = mutantStat.Score;
                mutantsByScore[score].Remove(mutant);
                if (mutantsByScore[score].Count == 0)
                {
                    mutantsByScore.Remove(score);
                }
                mutantStat.Score += change;
                mutantStat.ScorePer100Turns = mutantStat.Turns > 0 ? (double)mutantStat.Score * 100 / (double)mutantStat.Turns : 0;
                AddMutantScore(mutant, mutantStat.Score);
            }
        }

        private void AddMutantScore(int mutant, long score)
        {
            try
            {
                mutantsByScore.Add(score, new HashSet<int>());
            }
            catch (ArgumentException)
            {
            }
            mutantsByScore[score].Add(mutant);
        }

        private void SetRandomGoal(int carIndex)
        {
            var car = Cars[carIndex];

            for (; ; )
            {
                var pos = new Position((sbyte)random.Next(Map.Width), (sbyte)random.Next(Map.Height));
                if (Map[pos] == PaneState.Invalid || pos.Equals(car.Position))
                    continue;
                var diff = Math.Abs(pos.X - car.Position.X) + Math.Abs(pos.Y - car.Position.Y);
                if (diff < (Map.Width + Map.Height) / 4)
                    continue;
                car.Goal = pos;
                break;
            }
        }

        private Position GetRandomBlankPosition()
        {
            for (; ; )
            {
                var pos = new Position((sbyte)random.Next(Map.Width), (sbyte)random.Next(Map.Height));
                if (Map[pos] == PaneState.Blank)
                    return pos;
            }
        }

        public void Stop()
        {
            isStopped = true;
        }
    }

}
