using System.Threading.Tasks;
using Discord;

namespace TranslatorBot.Modules.Translation;

/// <summary>
///     The <see cref="TranslationModule" /> class is a static class which
///     contains methods for translating text. They are the commands which the bot can execute.
///     (Formerly, these were directly called by using prefix + command name, but now they are called by the <see cref="SlashCommandDispatcher" />.)
/// </summary>
public static class TranslationModule
{
    /// <summary>
    ///     Translate text from an automatically detected language to a specified target language.
    /// </summary>
    /// <param name="targetLanguage">
    ///     The language to translate to.
    /// </param>
    /// <param name="languageCode">
    ///     The language code of the command's user so that the bot can respond in the correct language.
    /// </param>
    /// <param name="text">
    ///     The text to be translated.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> containing an <see cref="Embed" /> which contains the translation result.
    /// </returns>
    public static async Task<Embed> Translate(string targetLanguage, string languageCode, params string[] text)
    {
        return await TranslateFrom("autodetect", targetLanguage, languageCode, text);
    }
    
    /// <summary>
    ///     Translate text from a specified source language to a specified target language.
    /// </summary>
    /// <param name="inputLanguage">
    ///     The language to translate from.
    /// </param>
    /// <param name="targetLanguage">
    ///     The language to translate to.
    /// </param>
    /// <param name="languageCode">
    ///     The language code of the command's user so that the bot can respond in the correct language.
    /// </param>
    /// <param name="text">
    ///     The text to be translated.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> containing an <see cref="Embed" /> which contains the translation result.
    /// </returns>
    public static async Task<Embed> TranslateFrom(string inputLanguage, string targetLanguage, string languageCode, params string[] text)
    {
        if (!TranslationService.IsTranslatorOperational)
        {
            Embed failedApiEmbed = EmbedGenerator.GenerateApiConnectionErrorEmbed(languageCode);
            return failedApiEmbed;
        }

        if (text.Length == 0)
        {
            Embed emptyTextEmbed = EmbedGenerator.GenerateEmptyTextEmbed(languageCode);
            return emptyTextEmbed;
        }

        if (await TranslationService.HasReachedCap())
        {
            Embed reachedLimitEmbed = EmbedGenerator.GenerateLimitReachedEmbed(languageCode);
            return reachedLimitEmbed;
        }

        string languageCodeSource = LanguageModelConversions.ConvertToLanguageCode(inputLanguage);
        string languageCodeDestination = LanguageModelConversions.ConvertToLanguageCode(targetLanguage);

        if (languageCodeSource.Length == 5)
            languageCodeSource = languageCodeSource.Remove(2, 3);

        if (languageCodeDestination == "UNKNOWN")
        {
            Embed unknownLanguageEmbed = EmbedGenerator.GenerateUnknownLanguageEmbed(targetLanguage, languageCode);
            return unknownLanguageEmbed;
        }
        else
        {
            string joinedText = string.Join(' ', text);
            (string translatedText, string detectedSourceLanguageCode) translationResult =
                await TranslationService.Translate(joinedText, languageCodeSource, languageCodeDestination);
            Embed translatedTextEmbedBuilder =
                EmbedGenerator.GenerateTranslationResultEmbed(languageCodeSource, languageCodeDestination,
                    languageCodeSource == "AUTOMATIC",
                    translationResult, languageCode);
            return translatedTextEmbedBuilder;
        }
    }

    /// <summary>
    ///     Reconnect to the DeepL API.
    /// </summary>
    /// <param name="languageCode">
    ///     The language code of the command's user so that the bot can respond in the correct language.
    /// </param>
    /// <returns>
    ///     A <see cref="Embed" /> which contains the result of the reconnection attempt.
    /// </returns>
    public static Embed ReconnectToTranslationApi(string languageCode)
    {
        bool isReconnectionSuccess = TranslationService.ReconnectToDeepL();
        Embed reconnectionAttemptResultEmbed = EmbedGenerator.GenerateReconnectionEmbed(isReconnectionSuccess, languageCode);
        return reconnectionAttemptResultEmbed;
    }
}