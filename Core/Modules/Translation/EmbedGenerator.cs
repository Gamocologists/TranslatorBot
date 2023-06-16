using System;
using System.Collections.Generic;
using Discord;
using TranslatorBot.Data.Translations;

namespace TranslatorBot.Modules.Translation;

/// <summary>
///     Contains methods for generating bot responses as embedded messages.
/// </summary>
public static class EmbedGenerator
{
    /// <summary>
    ///     Generates an embed for when the language is not recognized.
    /// </summary>
    /// <param name="languageInput">
    ///     The language provided and not recognized.
    /// </param>
    /// <param name="languageCode">
    ///     The language code which dictates the language of the embed.
    /// </param>
    /// <returns>
    ///     The generated embed.
    /// </returns>
    internal static Embed GenerateUnknownLanguageEmbed(string languageInput, string languageCode)
    {
        EmbedBuilder unknownLanguageEmbedBuilder = BuildBotEmbedBase();
        unknownLanguageEmbedBuilder.Color = Color.Red;
        
        string fieldName = LocalizationHandler.GetEmbedFieldTitle("Unknown Language", languageCode);
        string fieldValue = LocalizationHandler.GetEmbedFieldValue("Unknown Language", languageCode);

        EmbedFieldBuilder unknownLanguageField = new EmbedFieldBuilder
        {
            Name = fieldName,
            IsInline = true,
            Value = fieldValue
        };
        
        unknownLanguageEmbedBuilder.AddField(unknownLanguageField);

        Embed finalEmbed = unknownLanguageEmbedBuilder.Build();
        return finalEmbed;
    }

    /// <summary>
    ///     Generates a basic embed builder to be used in other embed generators.
    /// </summary>
    /// <returns>
    ///     A basic embed builder to be used in other embed generators.
    /// </returns>
    private static EmbedBuilder BuildBotEmbedBase()
    {
        EmbedBuilder botEmbedBase = new EmbedBuilder
        {
            Author = GenerateBotAuthor(),
            Color = Color.DarkBlue,
            Timestamp = DateTimeOffset.Now,
            Title = "Translation Results",
            //old link for translator image:
            //"https://cdns.c3dt.com/preview/9825728-com.deepl.translate.alllanguagetranslator.jpg"
            Url = "https://gamocologist.com/bots/gamotranslator"
        };
        return botEmbedBase;
    }

    /// <summary>
    ///     Generates the author information for use in embeds.
    /// </summary>
    /// <returns>
    ///     An embed author builder containing the author information for use in embeds.
    /// </returns>
    private static EmbedAuthorBuilder GenerateBotAuthor()
    {
        EmbedAuthorBuilder embedAuthorBuilder = new EmbedAuthorBuilder
        {
            Name = "GamoTranslate",
            IconUrl =
                "https://www.google.com/url?sa=i&url=https%3A%2F%2Fapitracker.io%2Fa%2Fdeepl&psig=AOvVaw028t84t0CBC87q4q-IuCMc&ust=1650642593447000&source=images&cd=vfe&ved=0CAwQjRxqFwoTCMDqzf3ApfcCFQAAAAAdAAAAABAO",
            Url = "https://gamocologist.com/bots/gamotranslator"
        };
        return embedAuthorBuilder;
    }

    /// <summary>
    ///     Generates a text containing the translation result
    /// </summary>
    /// <param name="languageCodeSource">
    ///     The language code of the language to be translated.
    /// </param>
    /// <param name="languageCodeDestination">
    ///     The language code of the language the text is translated to.
    /// </param>
    /// <param name="isAutomatic">
    ///     Indicates whether the <see cref="languageCodeSource" /> was detected automatically
    ///     by the translation engine. True if it was. False if it was not.
    /// </param>
    /// <param name="translationResult">
    ///     A tuple containing the result from the translation engine.
    ///     The first element of the tuple is a <see cref="string" /> which contains the translated text.
    ///     The second element of the tuple is a <see cref="string" /> which contains the language code of the detected
    ///     language of the text that was given to be translated.
    /// </param>
    /// <param name="languageCode">
    ///     The language code which dictates the language of the embed.
    /// </param>
    /// <returns>
    ///     Return an <see cref="Embed" /> that contains the translation result.
    /// </returns>
    internal static Embed GenerateTranslationResultEmbed(string languageCodeSource, string languageCodeDestination,
        bool isAutomatic, (string translatedText, string detectedSourceLanguageCode) translationResult, string languageCode)
    {
        EmbedBuilder translationResultEmbedBuilder = BuildBotEmbedBase();
        translationResultEmbedBuilder.Color = Color.DarkGreen;
        EmbedFieldBuilder translatedTextField = new EmbedFieldBuilder
        {
            IsInline = false
        };

        string destinationLanguage = LanguageModelConversions.ConvertToLanguageName(languageCodeDestination);
        string languageField;
        if (isAutomatic)
        {
            string detectedLanguage =
                LanguageModelConversions.ConvertToLanguageName(translationResult.detectedSourceLanguageCode);
            languageField = LocalizationHandler.GetEmbedCustomLocalization("TranslationResult", "field_title_automatically_detected.json", languageCode);
            languageField = languageField.Replace("%detectedLanguage%", detectedLanguage);
            languageField = languageField.Replace("%destinationLanguage%", destinationLanguage);
        }
        else
        {
            string sourceLanguage = LanguageModelConversions.ConvertToLanguageName(languageCodeSource);
            languageField = $"{sourceLanguage}** --> **{destinationLanguage}";
        }

        List<string> segmentedText = Utils.DivideUpTextIntoFragmentsNicely(
            translationResult.translatedText);
        int numberOfSegments = segmentedText.Count;
        translatedTextField.Name = LocalizationHandler.GetEmbedCustomLocalization("TranslationResult", "field_title.json", languageCode);
        translatedTextField.Name = translatedTextField.Name.Replace("%languageField%", languageField);
        if (numberOfSegments == 0)
            segmentedText.Add(" ");
        translatedTextField.Value = $"{segmentedText[0]}";
        translationResultEmbedBuilder.AddField(translatedTextField);

        for (int i = 1; i < numberOfSegments; i++)
        {
            EmbedFieldBuilder segmentFieldBuilder = new EmbedFieldBuilder
            {
                Name = "\u200b",
                IsInline = true,
                Value = segmentedText[i]
            };
            translationResultEmbedBuilder.AddField(segmentFieldBuilder);
        }

        Embed translationResultEmbed = translationResultEmbedBuilder.Build();
        return translationResultEmbed;
    }

    /// <summary>
    ///     Generates a message that indicates that the API's 500000 character/month translation limit has been reached.
    /// </summary>
    /// <param name="languageCode">
    ///     The language code which dictates the language of the embed.
    /// </param>
    /// <returns>
    ///     An <see cref="Embed" /> that indicates that the API's 500000 character/month translation limit has been reached.
    /// </returns>
    internal static Embed GenerateLimitReachedEmbed(string languageCode)
    {
        EmbedBuilder limitReachedEmbedBuilder = BuildBotEmbedBase();
        limitReachedEmbedBuilder.Color = Color.Orange;

        string nameField = LocalizationHandler.GetEmbedFieldTitle("TranslationLimitReached", languageCode);
        string valueField = LocalizationHandler.GetEmbedFieldValue("TranslationLimitReached", languageCode);

        EmbedFieldBuilder limitReachedField = new EmbedFieldBuilder
        {
            Name = nameField,
            IsInline = false,
            Value = valueField
        };
        
        limitReachedEmbedBuilder.AddField(limitReachedField);

        Embed limitReachedEmbed = limitReachedEmbedBuilder.Build();
        return limitReachedEmbed;
    }

    /// <summary>
    ///     Generates a response to a translation request where no text was provided for translation.
    /// </summary>
    /// <param name="languageCode">
    ///     The language code which dictates the language of the embed.
    /// </param>
    /// <returns>
    ///     An <see cref="Embed" /> containing a response to a translation request
    ///     where no text was provided for translation.
    /// </returns>
    internal static Embed GenerateEmptyTextEmbed(string languageCode)
    {
        EmbedBuilder limitReachedEmbedBuilder = BuildBotEmbedBase();
        limitReachedEmbedBuilder.Color = Color.Red;
        
        string nameField = LocalizationHandler.GetEmbedFieldTitle("EmptyText", languageCode);
        string valueField = LocalizationHandler.GetEmbedFieldValue("EmptyText", languageCode);

        EmbedFieldBuilder emptyTextFieldBuilder = new EmbedFieldBuilder
        {
            Name = nameField,
            IsInline = false,
            Value = valueField
        };

        limitReachedEmbedBuilder.AddField(emptyTextFieldBuilder);

        Embed generateEmptyTextEmbed = limitReachedEmbedBuilder.Build();
        return generateEmptyTextEmbed;
    }

    /// <summary>
    ///     Generates a message informing that the API wasn't successfully
    /// </summary>
    /// <param name="languageCode">
    ///     The language code which dictates the language of the embed.
    /// </param>
    /// <returns>
    ///     An <see cref="Embed" /> containing a message informing that the authentication
    ///     key was missing for the API.
    /// </returns>
    internal static Embed GenerateApiConnectionErrorEmbed(string languageCode)
    {
        EmbedBuilder apiErrorConnectionErrorEmbedBuilder = BuildBotEmbedBase();
        apiErrorConnectionErrorEmbedBuilder.Color = Color.DarkRed;
        
        string apiConnectionErrorTitle = LocalizationHandler.GetEmbedFieldTitle("ApiConnectionError", languageCode);
        string apiConnectionErrorValue = LocalizationHandler.GetEmbedFieldValue("ApiConnectionError", languageCode);

        EmbedFieldBuilder apiConnectionErrorFieldBuilder = new EmbedFieldBuilder
        {
            Name = apiConnectionErrorTitle,
            IsInline = false,
            Value = apiConnectionErrorValue
        };

        apiErrorConnectionErrorEmbedBuilder.AddField(apiConnectionErrorFieldBuilder);

        Embed apiConnectionErrorEmbed = apiErrorConnectionErrorEmbedBuilder.Build();
        return apiConnectionErrorEmbed;
    }

    /// <summary>
    ///     Generates a message informing that the API was or wasn't successfully reconnected.
    /// </summary>
    /// <param name="wasSuccessful">
    ///     A boolean indicating whether the reconnection was successful or not.
    /// </param>
    /// <param name="languageCode">
    ///     The language code which dictates the language of the embed.
    /// </param>
    /// <returns>
    ///     An <see cref="Embed" /> containing a message informing that the authentication
    ///     key was missing for the API.
    /// </returns>
    internal static Embed GenerateReconnectionEmbed(bool wasSuccessful, string languageCode)
    {
        EmbedBuilder reconnectionEmbedBuilder = BuildBotEmbedBase();
        
        string reconnectionTitle = wasSuccessful
            ? LocalizationHandler.GetEmbedFieldTitle("ReconnectionSuccess", languageCode)
            : LocalizationHandler.GetEmbedFieldTitle("ReconnectionFailure", languageCode);
        
        string reconnectionValue = wasSuccessful
            ? LocalizationHandler.GetEmbedFieldValue("ReconnectionSuccess", languageCode)
            : LocalizationHandler.GetEmbedFieldValue("ReconnectionFailure", languageCode);

        EmbedFieldBuilder reconnectionEmbedField = new EmbedFieldBuilder
        {
            Name = reconnectionTitle,
            IsInline = false,
            Value = reconnectionValue
        };

        reconnectionEmbedBuilder.Color = wasSuccessful ? Color.DarkGreen : Color.Red;
        reconnectionEmbedBuilder.AddField(reconnectionEmbedField);

        Embed reconnectionEmbed = reconnectionEmbedBuilder.Build();
        return reconnectionEmbed;
    }
}