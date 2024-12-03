using System.ComponentModel;
using System.Windows.Input;

namespace HangMann;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{

    private string spotLigth;
    public string SpotLigth
    {
        get => spotLigth;
        set
        {
            spotLigth = value;
            this.OnPropertyChanged();
        }
    }

    private List<char> alphabet = new List<char>();
    public List<char> Alphabet
    {
        get => alphabet;
        set
        {
            alphabet = value;
            this.OnPropertyChanged();
        }
    }

    private string message;
    public string Message
    {
        get => message;
        set
        {
            message = value;
            this.OnPropertyChanged();
        }
    }

    private string imgSource = "img0.jpg";
    public string ImgSource
    {
        get => imgSource; set
        {
            imgSource = value;
            this.OnPropertyChanged();
        }
    }

    private int maxMistakesCount = 5;

    private string attempts = $"Errors: 0 of 6";
    public string Attempts
    {
        get => attempts;
        set
        {
            attempts = value;
            this.OnPropertyChanged();
        }
    }

    private int mistakes = 0;

    private readonly List<string> words = new()
    {
        "net",
        "maui",
        "python",
        "angular"
    };

    private string answer = string.Empty;
    private List<char> inputChars = new();

    public MainPage()
    {
        InitializeComponent();
        this.InitializeAplhabet();
        this.InitializeAnswer();
        BindingContext = this;

        this.CalculateWord(this.answer, this.inputChars);
    }

    private void InitializeAplhabet()
    {
        this.Alphabet.AddRange("abcdefg");
    }

    private void InitializeAnswer()
    {
        var random = new Random();
        var next = random.Next(0, words.Count);
        this.answer = words[next];
    }

    private void CalculateWord(string word, List<char> chars)
    {
        var temp = word.Select(x => (chars.IndexOf(x) != -1) ? x : '_').ToArray();
        this.SpotLigth = string.Join(" ", temp);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            var letter = button.Text;
            button.IsEnabled = false;
            this.HandleInput(letter[0]);
        }

    }

    private void HandleInput(char v)
    {
        if (!this.inputChars.Contains(v))
        {
            inputChars.Add(v);
        }
        if (answer.Contains(v))
        {
            this.CalculateWord(this.answer, this.inputChars);
            this.CheckIfGameWon();
        }
        else if (!answer.Contains(v))
        {
            this.mistakes++;
            this.UpdateStatus();
            this.ChangeImage();
            this.CheckIfGameLost();
        }
    }

    private void ChangeImage()
    {
        this.ImgSource = $"img{this.mistakes}.jpg";
    }

    private void UpdateStatus()
    {
        this.Attempts = $"Errors: {mistakes} of {this.maxMistakesCount}";
    }

    private void CheckIfGameLost()
    {
        if (this.mistakes == this.maxMistakesCount)
        {
            this.Message = "You lost!";
            this.EnableAllButtons();
        }
    }

    private void EnableAllButtons(bool state = false)
    {
        foreach (var item in Conatiner.Children)
        {
            if (item is Button button)
            {
                button.IsEnabled = state;
            }
        }
    }

    private void CheckIfGameWon()
    {
        if (this.SpotLigth.Replace(" ", "") == this.answer)
        {
            this.Message = "You are win";
            this.EnableAllButtons();
        }
    }

    private void Reset_Clicked(object sender, EventArgs e)
    {
        this.mistakes = 0 ;
        this.inputChars = new List<char>();
        this.Message = string.Empty;
        this.ImgSource = "img0.jpg";
        this.Attempts = $"Errors: 0 of 6";
        this.InitializeAplhabet();
        this.InitializeAnswer();
        this.EnableAllButtons(true);
        this.CalculateWord(this.answer, this.inputChars);
    }
}

