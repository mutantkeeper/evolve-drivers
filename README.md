# Brute-force Evolution

## Ideology
Brute-force Evolution is based on 2 beliefs.

- Human intelligence is generated by evolution
- True machine intelligence shall not be designed by human.

## History

In May 2000, I realized that the way how computer works at the low level is very different than human brain, and there are something that computer easily beats human even with very old and slow hardware like calculation. Trying to make computer work in the way how human brain works limits the potential of machine intelligence. While I believed human intelligence is generated by evolution, I naturally drew a conclusion that the true machine intelligence should come from evolution, too.

Then what to evolve? A CPU works in the way of executing instructions. So ideally it should be sequences of CPU instructions that are being treated as organisms. And I need an environment to run those organisms and do the natual selection based on some criteria. However, I would not go that far at the beginning. What I did is defining some instructions like ADD, SUBTRACT, AND, OR, ...blabla, and making the sequences work on a small piece of memory. It is still much different than how CPU works. There is no registers, jumpps, function calls, or interrupts, but good enough to start experiment.

I started experiment immediately with Tic Tac Toe. The result was quite good. I lost all the source code though.

In December 2017, I redid this experiment. While the computer is much faster now, so I easily got some routines that can play quite well. However, I realied that it is very hard to get a routine that plays perfectly (always plays best move in any situation). That may be fine, because it is hard to find a perfect human being either. This brute-force evolution is probably not good at board games.

I always want to run this kind of evolution in an environment like our road traffic, and see if good driving culture can be generated by evolution. While I talked with my daughter about how to evaluate the driving, she got an idea that the drivers can be randomly assigned destinations. And this is why this project is created.

## Mutants

I call the routines that are being evolved **Mutants**. Mutants may have their static data. In this project, a mutant is just a seqeunce of instructions.

When a mutant is executed, it is given a piece of memory. Some area of the memory is input data, and some area (usually the first byte) is output. Some of memory can be relatively persistent (exclusive for every mutant), and can be used by every mutant to store something it can read next time.

An instruction contains an op code and operands, and flags indicating if operands are immediate values or memory addresses.

## Project evolve-drivers

To be simple, a map is a grid which width and height are equal. Every driver (mutant) drives a car, and every car can only stay on a pane on the map. In each turn, every driver makes an action which is move ahead, turn left, turn right, turn back or stay. When a car is moving into a pane where is already occupied by something, the collision happens, and the car will say. When multiple cars try to move into an empty pane in the same turn, the collision happens, and all of them stay at their current positions.

## Results

### Environment

**Only 100** mutants each of which has **only 256** instructions (or 1024 bytes). Map size used to evolve is 40 x 40.

### New

I restarted evolution from scratch. And this time it took only **10 minutes** to generate mutants that can drive to the goal.

There are 2 major changes made recently:
- persistent memory
- Jump instructions.

After less than 40 minutes: https://youtu.be/7DioIDzWgO4
