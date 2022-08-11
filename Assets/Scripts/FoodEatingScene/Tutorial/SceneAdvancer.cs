using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneAdvancer : MonoBehaviour
{
    public Text information;
    public Button nextButton;
    public Button backButton;
    public int stage;

    public GameObject agent;
    public GameObject arrow;
    public GameObject food;
    public GameObject wall;

    public GameObject Line1;
    public GameObject Line2;
    public GameObject Line3;
    public GameObject Line4;

    public GameObject map1;
    public GameObject map2;
    public GameObject map3;
    public GameObject map4;
    public GameObject map5;

    public bool goingForward;
    
    // Start is called before the first frame update
    void Start()
    {
        information.text = "This is the learning environment";
        try{
			nextButton.GetComponent<Button>().onClick.AddListener(advanceScene);
		} catch {
			UnityEngine.Debug.Log("no next button");
		}
        try{
			backButton.GetComponent<Button>().onClick.AddListener(deAdvanceScene);
		} catch {
			UnityEngine.Debug.Log("no next button");
		}
        stage = 0;
    }

    void advanceScene()
    {
        if(!goingForward){
            stage = stage + 1;
            goingForward = true;
        }
        if(stage < 11)
        {
            UnityEngine.Debug.Log("next");
            showStep();
            stage = stage + 1;
        }
        
        UnityEngine.Debug.Log(stage);
    }

    void deAdvanceScene()
    {
        if(goingForward){
            stage = stage - 1;
            goingForward = false;
        }
        if(stage == 9)
        {
            stage = stage - 1;
            UnityEngine.Debug.Log("back");
            step9_();
        }
        else if(stage > 0)
        {
            stage = stage - 1;
            showStep();
            UnityEngine.Debug.Log("back");
        }
        
        UnityEngine.Debug.Log(stage);
    }

    void showStep()
    {
        if(stage == 0)
        {
            step1();
        } else if (stage == 1)
        {
            step2();
        } else if (stage == 2)
        {
            step3();
        } else if (stage == 3)
        {
            step4();
        } else if (stage == 4)
        {
            step5();
        } else if (stage == 5)
        {
            step6();
        } else if (stage == 6)
        {
            step7();
        } else if (stage == 7)
        {
            step8();
        } else if (stage == 8)
        {
            step9();
        } else if (stage == 9)
        {
            step10();
        } 
        
        
    }

    void Update()
    {
        //UnityEngine.Debug.Log(Application.dataPath + "/example.bat");
        if(stage>1)
        {
            Debug.DrawLine(agent.transform.position, food.transform.position, Color.green);
        }

        
    }

    void step1()
    {
        information.text = "This here is the agent";
        arrow.transform.localPosition = new Vector3(-200,-420,0);
    }

    void step2()
    {
        information.text = "This here is a piece of food. In this environment the agents goal is to move to the food."
        + " Moving to the food will produce energy.";
        arrow.transform.localPosition = new Vector3(100,-150,0);
    }

    void step3()
    {
        information.text = "The location of the piece of food is sent to the neural network in the form of a float Vector2"
        + " that makes up 1 part of the input";
        arrow.transform.localPosition = new Vector3(830,350,0);
        map1.transform.localPosition = new Vector3(9999,9999,0);
    }

    void step4()
    {
        information.text = "The white blocks are wall pieces that surround the environment." 
        +  " If the agent collides with a piece of wall it is counted as a failure. " +
        "In this example the white line represents the view of the walls. Each direction" + 
        " is returned to the model";
        arrow.transform.localPosition = new Vector3(-500,-560,0);
        Line1.transform.localPosition = new Vector3(175,-300,0);
        Line2.transform.localPosition = new Vector3(-875,-300,0);
        Line3.transform.localPosition = new Vector3(-200,-425,0);
        Line4.transform.localPosition = new Vector3(-200,125,0);
    }

    void step5()
    {
        information.text = "The distance to the 4 walls are passed into the neural network as a set of 4 floats";
        arrow.transform.localPosition = new Vector3(830,150,0);
        map2.transform.localPosition = new Vector3(9999,9999,0);
    }

    void step6()
    {
        information.text = "This is the energy level. The energy level is increased by eating food."
        + " The agents goal is to maximise the energy level until it reaches double what it started with";
        arrow.transform.localPosition = new Vector3(-750,450,0);
    }

    void step7()
    {
        information.text = "All of these inputs are fed into the hidden layers. " +
        "Unlike the image (too complex to show) every input is fed to every node into every node of the first hidden layer";
        map4.transform.localPosition = new Vector3(9999,9999,0);
        arrow.transform.localPosition = new Vector3(1150,-60,0);
    }

    void step8()
    {
        information.text = "The Hidden layers then process the inputs using complex maths and translates into two outputs.";
        arrow.transform.localPosition = new Vector3(1650,150,0);
        map5.transform.localPosition = new Vector3(9999,9999,0);
    }

    void step9()
    {
        agent.transform.position = agent.transform.position + new Vector3(30,30,0);
        information.text = "The two outputs are then translated into a movement X and a movement Y direction."
        + " The agent is then translated by the movement X and movement Y.";
        Line1.transform.localPosition = Line1.transform.localPosition + new Vector3(30,30,0);
        Line2.transform.localPosition = Line2.transform.localPosition + new Vector3(30,30,0);
        Line3.transform.localPosition = Line3.transform.localPosition + new Vector3(30,30,0);
        Line4.transform.localPosition = Line4.transform.localPosition + new Vector3(30,30,0);
        arrow.transform.localPosition = agent.transform.position + new Vector3(0,-100,0);
    }

    void step9_()
    {
        agent.transform.position = agent.transform.position + new Vector3(-30,-30,0);
        Line1.transform.localPosition = Line1.transform.localPosition + new Vector3(-30,-30,0);
        Line2.transform.localPosition = Line2.transform.localPosition + new Vector3(-30,-30,0);
        Line3.transform.localPosition = Line3.transform.localPosition + new Vector3(-30,-30,0);
        Line4.transform.localPosition = Line4.transform.localPosition + new Vector3(-30,-30,0);
        arrow.transform.localPosition = agent.transform.position + new Vector3(0,-100,0);
    }

    void step10()
    {
        information.text = "The reward gained from these actions is then fed through the neural " +
        "network to update the function parameters in the hidden layer. This cycle of collection is then repeated again." +
        " But what does this look like?.";
    }
}
