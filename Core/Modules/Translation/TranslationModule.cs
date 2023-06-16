﻿using System.Threading.Tasks;
using Discord;

namespace TranslatorBot.Modules.Translation;

public static class TranslationModule
{
    public static async Task<Embed> Translate(string targetLanguage, string languageCode, params string[] text)
    {
        return await TranslateFrom("autodetect", targetLanguage, languageCode, text);
    }
    
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

    public static Embed ReconnectToTranslationApi(string languageCode)
    {
        bool isReconnectionSuccess = TranslationService.ReconnectToDeepL();
        Embed reconnectionAttemptResultEmbed = EmbedGenerator.GenerateReconnectionEmbed(isReconnectionSuccess, languageCode);
        return reconnectionAttemptResultEmbed;
    }
}