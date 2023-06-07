using System.Threading.Tasks;
using Discord;

namespace TranslatorBot.Modules.Translation;

public class TranslationModule
{
    public async Task<Embed> Translate(string targetLanguage, params string[] text)
    {
        return await TranslateFrom("autodetect", targetLanguage, text);
    }
    
    public async Task<Embed> TranslateFrom(string inputLanguage, string targetLanguage, params string[] text)
    {
        if (!TranslationService.IsTranslatorOperational)
        {
            Embed failedApiEmbed = EmbedGenerator.GenerateApiConnectionErrorEmbed();
            return failedApiEmbed;
        }

        if (text.Length == 0)
        {
            Embed emptyTextEmbed = EmbedGenerator.GenerateEmptyTextEmbed();
            return emptyTextEmbed;
        }

        if (await TranslationService.HasReachedCap())
        {
            Embed reachedLimitEmbed = EmbedGenerator.GenerateLimitReachedEmbed();
            return reachedLimitEmbed;
        }

        string languageCodeSource = LanguageModelConversions.ConvertToLanguageCode(inputLanguage);
        string languageCodeDestination = LanguageModelConversions.ConvertToLanguageCode(targetLanguage);

        if (languageCodeSource.Length == 5)
            languageCodeSource = languageCodeSource.Remove(2, 3);

        if (languageCodeDestination == "UNKNOWN")
        {
            Embed unknownLanguageEmbed = EmbedGenerator.GenerateUnknownLanguageEmbed(targetLanguage);
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
                    translationResult);
            return translatedTextEmbedBuilder;
        }
    }

    public Embed ReconnectToTranslationApi()
    {
        bool isReconnectionSuccess = TranslationService.ReconnectToDeepL();
        Embed reconnectionAttemptResultEmbed = EmbedGenerator.GenerateReconnectionEmbed(isReconnectionSuccess);
        return reconnectionAttemptResultEmbed;
    }
}