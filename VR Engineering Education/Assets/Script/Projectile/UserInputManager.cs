using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInputManager : MonoBehaviour
{
    public Canvas canvas;
    public Text question;
    public Text answer;
    public List<GameObject> gameObjects = new List<GameObject>(3);
    public static UserInputManager manager;

    public Text Vx;
    public Text Vy;
    public Text Vo;
    // Start is called before the first frame update
    void Start()
    {
        manager = this;
        //canvas.gameObject.SetActive(false);
        updateQuestion();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void selectedField(string name)
    {
        foreach (GameObject i in gameObjects)
        {
            if (!i.name.Equals(name))
            {
                i.SetActive(false);
            }
        }
    }

    public void actvieAll()
    {
        foreach (GameObject i in gameObjects)
        {
            i.SetActive(true);
        }
    }

    public void updateQuestion()
    {
        Trajectory.trajectoryManager.randomizeQuestion();
        updatePrompt();
    }

    public void updatePrompt()
    {
        question.text = "Given the distance of " + Trajectory.trajectoryManager.givenDistanceX + " from point A to B. Calculate the initial velocity of the ball given the maximum height is "
            + Trajectory.trajectoryManager.givenHeight + ", the flight time is " + Trajectory.trajectoryManager.givenTime + " The height difference between the original point and target is " + Trajectory.trajectoryManager.givenDistanceY
            + " and the angle is " + Trajectory.trajectoryManager.angle + ". Please round to 2 decimal places.";
        Vx.text = string.Empty;
        Vy.text = string.Empty;
        Vo.text = string.Empty;
        /*
        answer.text = "Correct Vx = " + (float)Math.Round(Trajectory.trajectoryManager.correctVx, 2) + ". Correct Vy = " + (float)Math.Round(Trajectory.trajectoryManager.correctVy, 2) + ". Correct Vo = "
            + (float)Math.Round(Trajectory.trajectoryManager.correctVo, 2) + ".";
        */
    }

    //public void checkAnswer()
    //{
    //    // Not working, X and Z is for direction left and right. Please refer to document
    //    /*
    //    float studentVx;
    //    float.TryParse(Vx.text, out studentVx);
    //    */
    //    // End of failure

    //    float studentVy;
    //    float.TryParse(Vy.text, out studentVy);
    //    float studentVo;
    //    float.TryParse(Vo.text, out studentVo);

    //    // Not working, X and Z is for direction left and right. Please refer to document
    //    /*
    //    studentVx = (float)Math.Round(studentVx, 2);
    //    */
    //    // End of failure

    //    studentVy = (float)Math.Round(studentVy, 2);
    //    studentVo = (float)Math.Round(studentVo, 2);

    //    // float correctVx = (float)Math.Round(Trajectory.trajectoryManager.correctVx, 2);
    //    float correctVy = (float)Math.Round(Trajectory.trajectoryManager.correctVy, 2);
    //    float correctVo = (float)Math.Round(Trajectory.trajectoryManager.correctVo, 2);

    //    /*
    //    if (studentVx == correctVx)
    //    {
    //        Vx.color = Color.magenta;
    //    }
    //    */

    //    if (studentVy == correctVy)
    //    {
    //        Vy.color = Color.magenta;
    //    }

    //    if (studentVo == correctVo)
    //    {
    //        Vo.color = Color.magenta;
    //    }
    //}
}
