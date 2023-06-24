using System.Text;

namespace TranslatorBot;

/// <summary>
///     A class containing extension methods for <see cref="string" />.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Removes discord mentions from a string.
    /// </summary>
    /// <param name="text">
    ///     The string to remove mentions from.
    /// </param>
    /// <returns>
    ///     The string without mentions.
    /// </returns>
    public static string RemoveMentions(this string text)
    {
        StringBuilder newTextBuilder = new(text);

        int index = 0;
        int length = newTextBuilder.Length;
        
        while (index < length)
        {
            char currentChar = newTextBuilder[index];
            if (IsMention(text, index))
            {
                newTextBuilder.Append("`@`");
            }
            else
            {
                newTextBuilder.Append(currentChar);
            }
            
            index += 1;
        }
        
        string newText = newTextBuilder.ToString();
        return newText;
    }
    
    /// <summary>
    ///     Checks if a character is a mention.
    /// </summary>
    /// <param name="text">
    ///     The string to check.
    /// </param>
    /// <param name="index">
    ///     The index of the character to check.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if the character is a the start of a mention, <see langword="false" /> otherwise.
    /// </returns>
    private static bool IsMention(string text, int index)
    {
        if (text[index] is not '@')
        {
            return false;
        }
        
        if (index - 1 == -1)
        {
            return true;
        }

        if (text[index - 1] is '<')
        {
            return true;
        }
        
        if (text[index - 1] is ' ')
        {
            return index + 1 < text.Length && text[index + 1] is not ' ';
        }

        return false;
    }
}