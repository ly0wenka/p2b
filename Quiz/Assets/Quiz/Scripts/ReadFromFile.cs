using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fungus;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Quiz.Scripts
{
    public class ReadFromFile : MonoBehaviour
    {
        private string _text;
        private string[] _lines;
        private string _lineQuestion;
        private string _lineCorrectAnswer;
        private string[] _wordsCorrectAnswer;
        private Flowchart _flowchart;
        private int _count = 0;
        private int _chosen = 60;
        private int _minChoosen = 60;
        private int MaxChoosen => _lines.Length;
        private int RandomChoosen => Random.Range(_minChoosen, MaxChoosen);
        private int RandomOddChoosen
        {
            get
            {
                var randomChoosen = RandomChoosen;
                return randomChoosen + (randomChoosen % 2 == 0 ? 0 : 1);
            }
        }

        private Dictionary<string, int> filesMinChosens = new Dictionary<string, int>
        {
            {"constructions_with_koto", 60},
            {"degrees_of_politeness", 250},
            {"homogeneous", 220},
            {"homogeneous_members_of_a_sentence", 220},
            {"loss_indicators", 144},
            {"modal_constructions", 70},
            {"modal_constructions2", 180},
            {"negative", 188},
            {"politeness", 258},
            {"pronouns2", 140},
            {"sentence_interrogative", 148},
            {"sentence_negative", 182},
            {"sentence_question", 148},
        };

        private string _filepath;

        private void SetRandomFileMinChoose()
        {
            var randomFileMinChoose = filesMinChosens.ElementAt(Random.Range(0, filesMinChosens.Count));
            _minChoosen = randomFileMinChoose.Value;
            _filepath = randomFileMinChoose.Key;
        }

        public void AfterAnswerRead()
        {
            CorrectAnswersSet();
            if (_count == _wordsCorrectAnswer.Length)
            {
                _count = 0;
                ChangeChoosen();
                Choose();
                SplitOnWordsCorrectAnswer();
                QuestionSet();
                GoToQuestion();
                CorrectAnswersSet();
            }
        }

        private void ChangeChoosen()
        {
            _chosen = RandomOddChoosen;
        }

        private void GoToQuestion()
        {
            _flowchart.StopAllBlocks();
            _flowchart.ExecuteBlock("Question");
        }

        // Start is called before the first frame update
        void Start()
        {
            SetRandomFileMinChoose();
            Debug.Log(_filepath+" "+_minChoosen);
            Read();
            Split();
            ChangeChoosen();
            Choose();
            SplitOnWordsCorrectAnswer();
            
            GetFlowchart();
            QuestionSet();
            CorrectAnswersSet();
        }

        private void CorrectAnswersSet()
        {
            var word = _wordsCorrectAnswer[_count];
            (_flowchart.FindBlock("A").CommandList[0] as Say).SetStandardText(word);
            (_flowchart.FindBlock("Menu Answers").CommandList[0] as Menu).SetStandardText(word);
            _count++;
        }

        private void QuestionSet() => (_flowchart.FindBlock("Question").CommandList[0] as Say).SetStandardText(_lineQuestion);

        private void SplitOnWordsCorrectAnswer() => _wordsCorrectAnswer = _lineCorrectAnswer.Split(" ");

        private void GetFlowchart() => _flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();

        private void Choose()
        {
            _lineQuestion = _lines[_chosen];
            Debug.Log(_chosen);
            _lineCorrectAnswer = _lines[_chosen - 1];
            Debug.Log(_chosen);
            Debug.Log(_lineQuestion);
            Debug.Log(_lineCorrectAnswer);
        }

        private void Split() => _lines = _text.Split("\n");
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private void Read()
        {
            var assetsHelp = $"Help/{_filepath}";
            var textAsset = Resources.Load<TextAsset>(assetsHelp);
            _text = textAsset.text;
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}