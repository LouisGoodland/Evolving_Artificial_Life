using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSequencer : MonoBehaviour
{
    public Button AIExplainButton;
    public Button machineLearningExplainButton;
    public Button reinforcementLearningExplainButton;
    public Button deepLearningExplainButton;
    public Button neuralNetworkExplainButton;

    public Text infoBox;
    // Start is called before the first frame update
    void Start()
    {
        infoBox.text = "test";

        try{
            AIExplainButton.GetComponent<Button>().onClick.AddListener(explainAI);
        } catch {
            UnityEngine.Debug.Log("error with AI explain");
        }

        try{
            machineLearningExplainButton.GetComponent<Button>().onClick.AddListener(explainMachineLearning);
        } catch {
            UnityEngine.Debug.Log("error with machine learning button");
        }

        try{
            reinforcementLearningExplainButton.GetComponent<Button>().onClick.AddListener(explainReinforcementLearning);
        } catch {
            UnityEngine.Debug.Log("error with machine learning button");
        }

        try{
            deepLearningExplainButton.GetComponent<Button>().onClick.AddListener(explainDeepLearning);
        } catch {
            UnityEngine.Debug.Log("error with machine learning button");
        }

        try{
            neuralNetworkExplainButton.GetComponent<Button>().onClick.AddListener(explainNeuralNetworks);
        } catch {
            UnityEngine.Debug.Log("error with machine learning button");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void explainMachineLearning()
    {
        infoBox.text = "Machine learning was first used in 1952 by Arthur Samuel, an engineer from IBM who used a learning algorithm to play checkers. It is a subfield of AI that fits under the limited memory category. It is 'the field of study that gives computers the ability to learn without explicitly being programmed'. Machine learning first requires a data collection process then after a model is trained to perform a task with the data. There are 3 categories of machine learning: supervised, unsupervised and reinforcement. The area of focus for this project is reinforcement learning";

    }

    void explainReinforcementLearning()
    {
        infoBox.text = "Reinforcement learning is the act of training machine learning models to make a number of complex actions in an uncertain environment. A very easy way to explain it is reinforcement learning is often used to play video games. In a reinforcement learning environment a user will specify a reward policy e.g. what actions in the environment are good and what actions are bad. The reinforcement learning agent's goal is to maximise the reward gained by producing an action policy. This is done with a trial and error approach. Typically a reinforcement learning environment will make use of a neural network model in order to complete the task."; 
    }

    void explainAI()
    {
        infoBox.text = "Artificial intelligence is a large area of computer science that focuses on performing tasks that are perceived as requiring human intelligence. A few examples of such are self driving cars, conversational bots and email spam filters. There are 4 types of artificial intelligence: Reactive machines, Limited memory, Theory of mind and Self awareness. Reactive machines are the most basic type and don’t have any memory or ability to influence current choices based on past choices. Limited memory machines can use the past and adapt their methodology with the information collected. Theory of mind and self awareness are concepts that are not quite achieved with today's technology that involve the AI being self aware and having a concept of others.";
    }

    void explainDeepLearning()
    {
        infoBox.text = "Deep Learning is a subsection of machine learning that specifically uses complex neural networks to achieve its task";
    }

    void explainNeuralNetworks()
    {
        infoBox.text = "A neural network is a collection of algorithms whose goal is to recognise patterns in sets of data by mimicking the way a human brain works. Each neuron in the network is a mathematical function that classifies data according to its architecture. A neural network has multiple neurons connected to each other. The input layer will have its inputs directly from the data e.g. it could be the distance to an object. The output layer will be translated into a result or an action";
    }
}
