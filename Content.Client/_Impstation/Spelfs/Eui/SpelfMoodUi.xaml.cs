using Content.Client.UserInterface.Controls;
using Content.Shared._Impstation.Spelfs;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Utility;

namespace Content.Client._Impstation.Spelfs.Eui;

[GenerateTypedNameReferences]
public sealed partial class SpelfMoodUi : FancyWindow
{
    private List<SpelfMood> _moods = new();

    public SpelfMoodUi()
    {
        RobustXamlLoader.Load(this);
        NewMoodButton.OnPressed += _ => AddNewMood();
    }

    private void AddNewMood()
    {
        MoodContainer.AddChild(new MoodContainer());
    }

    public List<SpelfMood> GetMoods()
    {
        var newMoods = new List<SpelfMood>();

        foreach (var control in MoodContainer.Children)
        {
            if (control is not MoodContainer moodControl)
                continue;

            if (string.IsNullOrWhiteSpace(moodControl.SpelfMoodTitle.Text))
                continue;

            var moodText = Rope.Collapse(moodControl.SpelfMoodContent.TextRope).Trim();

            if (string.IsNullOrWhiteSpace(moodText))
                continue;

            var mood = new SpelfMood()
            {
                MoodName = moodControl.SpelfMoodTitle.Text,
                MoodDesc = moodText,
            };

            newMoods.Add(mood);
        }

        return newMoods;
    }

    private void MoveUp(int index)
    {
        if (index <= 0)
            return;

        (_moods[index], _moods[index - 1]) = (_moods[index - 1], _moods[index]);
        SetMoods(_moods);
    }

    private void MoveDown(int index)
    {
        if (index >= _moods.Count - 1)
            return;

        (_moods[index], _moods[index + 1]) = (_moods[index + 1], _moods[index]);
        SetMoods(_moods);
    }

    private void Delete(int index)
    {
        _moods.RemoveAt(index);

        SetMoods(_moods);
    }

    public void SetMoods(List<SpelfMood> moods)
    {
        _moods = moods;
        MoodContainer.RemoveAllChildren();
        for (var i = 0; i < moods.Count; i++)
        {
            var index = i; // Copy for the closure
            var mood = moods[i];
            var moodControl = new MoodContainer(mood);
            moodControl.MoveUp.OnPressed += _ => MoveUp(index);
            moodControl.MoveDown.OnPressed += _ => MoveDown(index);
            moodControl.Delete.OnPressed += _ => Delete(index);
            MoodContainer.AddChild(moodControl);
        }
    }
}
