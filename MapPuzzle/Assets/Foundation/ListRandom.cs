using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListRandom {

    public static List<T> GetRandomList<T>(List<T> inputList)
    {
        //Set outputList and random
        List<T> outputList = new List<T>();
        System.Random rd = new System.Random(DateTime.Now.Millisecond);

        while (inputList.Count > 0)
        {
            //Select an index and item
            int rdIndex = rd.Next(0, inputList.Count - 1);
            T remove = inputList[rdIndex];

            //remove it from copyList and add it to output
            inputList.Remove(remove);
            outputList.Add(remove);
        }
        return outputList;
    }

}
