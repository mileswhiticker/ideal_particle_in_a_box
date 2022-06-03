using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public partial class SimController : MonoBehaviour
{
    void Start()
    {
        ApplicationStartTime = System.DateTime.UtcNow;
        Log.logID = "" + ApplicationStartTime.Day + "_" + ApplicationStartTime.Month + "_" + ApplicationStartTime.Year + "_" + ApplicationStartTime.Ticks;
        Log.AddLine("Sim started " + ApplicationStartTime);

        //what widths are we testing?
        /*
        trialWidths.Add(0.5f);
        trialWidths.Add(1);
        trialWidths.Add(1.5f);
        trialWidths.Add(2);
        trialWidths.Add(2.5f);
        */
        trialWidths.Add(6f);

        //output our initial data
        /*for (int i = 0; i < trialWidths.Count; i++)
        {
            for (int j = 0; j < trialMax; j++)
            {
                Log.AddLine("" + trialWidths[i]);
            }
        }*/

        //start with this one
        curLength = trialWidths[0];

        Initialize();
        endTimeText.text = "Sim end time: " + trialWidths.Count * trialMax * timeMax;
        isSimRunning = true;
    }
}
