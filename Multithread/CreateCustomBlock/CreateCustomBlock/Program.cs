using System;
using System.Threading.Tasks.Dataflow;

namespace CreateCustomBlock
{
    class Program
    {
        static IPropagatorBlock<int, int> CustomBlock()
        {
            var multiplyBlock = new TransformBlock<int,int>(item => item * 4);
            var addBlock = new TransformBlock<int,int>(item => item + 20);
            var divideBlock = new TransformBlock<int, int>(item => item / 2);

            var flowCompletion = new DataflowLinkOptions { PropagateCompletion = true };
            multiplyBlock.LinkTo(addBlock, flowCompletion);
            addBlock.LinkTo(divideBlock, flowCompletion);

            return DataflowBlock.Encapsulate(multiplyBlock, divideBlock);
        }

        static void Main(string[] args)
        {
            var customBlock = CustomBlock();
            for (int i = 0; i < 30; i++)
            {
                customBlock.Post(i);
            }
            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine(customBlock.Receive());
            }
        }
    }
}
