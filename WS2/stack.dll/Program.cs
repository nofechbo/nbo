class StackTest
{
    static void Main()
    {
        MyStack stack = new MyStack(5);

        Console.WriteLine(stack.IsEmpty());

        stack.Push(5);
        stack.Push("hello");
        stack.Push(true);

        Console.WriteLine(stack.IsEmpty());

        while (!stack.IsEmpty())
        {
            Console.WriteLine(stack.Pop());
        }

        stack.Push(5);
        stack.Push("hello");
        stack.Push(true);

        while (!stack.IsEmpty())
        {
            Console.WriteLine(stack.Peek());
            stack.Pop();
        }

        stack.Push(5);
        stack.Push("hello");
        Console.WriteLine("\n" + stack.IsEmpty());
        stack.Clear();
        Console.WriteLine(stack.IsEmpty());

        
    }
}