using System;

public class MyStack
{
	object[] objects;
	int curr_idx;

	public MyStack(int? capacity)
	{
		this.objects = new object[capacity ?? 10];
		curr_idx = 0;
	}

	public void Push(object obj)
	{
		objects[curr_idx++] = obj;
	}

	public object Pop()
	{
		object retVal = objects[0];

		Array.Copy(objects, 1, objects, 0, curr_idx);

		curr_idx--;
		return retVal;
	}

	public object Peek() { 
		return objects[0];
	}

	public bool IsEmpty() { 
		return curr_idx == 0;
	}

	public void Clear()
	{
		Array.Clear(objects, 0, objects.Length);	
		curr_idx = 0;
	}
}
