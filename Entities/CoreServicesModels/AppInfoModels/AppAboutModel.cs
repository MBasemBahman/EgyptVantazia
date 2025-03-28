﻿namespace Entities.CoreServicesModels.AppInfoModels
{
    public class AppAboutModel : AuditEntity
    {
        [DisplayName(nameof(AboutCompany))]
        [DataType(DataType.MultilineText)]
        public string AboutCompany { get; set; }

        [DisplayName(nameof(AboutApp))]
        [DataType(DataType.MultilineText)]
        public string AboutApp { get; set; }

        [DisplayName(nameof(TermsAndConditions))]
        [DataType(DataType.MultilineText)]
        public string TermsAndConditions { get; set; }

        [DisplayName(nameof(QuestionsAndAnswer))]
        [DataType(DataType.MultilineText)]
        public string QuestionsAndAnswer { get; set; }

        [DisplayName(nameof(GameRules))]
        [DataType(DataType.MultilineText)]
        public string GameRules { get; set; }

        [DisplayName(nameof(Subscriptions))]
        [DataType(DataType.MultilineText)]
        public string Subscriptions { get; set; }

        [DisplayName(nameof(Prizes))]
        [DataType(DataType.MultilineText)]
        public string Prizes { get; set; }

        [DisplayName(nameof(Phone))]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string Phone { get; set; }

        [DisplayName(nameof(WhatsApp))]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string WhatsApp { get; set; }

        [DisplayName(nameof(EmailAddress))]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [DisplayName(nameof(TwitterUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string TwitterUrl { get; set; }

        [DisplayName(nameof(FacebookUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string FacebookUrl { get; set; }

        [DisplayName(nameof(InstagramUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string InstagramUrl { get; set; }

        [DisplayName(nameof(SnapChatUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string SnapChatUrl { get; set; }

        [DisplayName(nameof(TiktokUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string TiktokUrl { get; set; }

        [DisplayName(nameof(TelegramUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string TelegramUrl { get; set; }

        [DisplayName(nameof(YoutubeUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string YoutubeUrl { get; set; }

        [DisplayName(nameof(ShowPayment))]
        public bool ShowPayment { get; set; }

        [DisplayName(nameof(ShowInvite))]
        public bool ShowInvite { get; set; }
    }

    public class AppAboutCreateOrEditModel
    {
        [DisplayName(nameof(AboutCompany))]
        [DataType(DataType.MultilineText)]
        public string AboutCompany { get; set; }

        [DisplayName(nameof(AboutApp))]
        [DataType(DataType.MultilineText)]
        public string AboutApp { get; set; }

        [DisplayName(nameof(TermsAndConditions))]
        [DataType(DataType.MultilineText)]
        public string TermsAndConditions { get; set; }

        [DisplayName(nameof(QuestionsAndAnswer))]
        [DataType(DataType.MultilineText)]
        public string QuestionsAndAnswer { get; set; }

        [DisplayName(nameof(GameRules))]
        [DataType(DataType.MultilineText)]
        public string GameRules { get; set; }

        [DisplayName(nameof(Subscriptions))]
        [DataType(DataType.MultilineText)]
        public string Subscriptions { get; set; }

        [DisplayName(nameof(Prizes))]
        [DataType(DataType.MultilineText)]
        public string Prizes { get; set; }

        [DisplayName(nameof(Phone))]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string Phone { get; set; }

        [DisplayName(nameof(WhatsApp))]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string WhatsApp { get; set; }

        [DisplayName(nameof(EmailAddress))]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [DisplayName(nameof(TwitterUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string TwitterUrl { get; set; }

        [DisplayName(nameof(FacebookUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string FacebookUrl { get; set; }

        [DisplayName(nameof(InstagramUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string InstagramUrl { get; set; }

        [DisplayName(nameof(SnapChatUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string SnapChatUrl { get; set; }

        [DisplayName(nameof(TiktokUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string TiktokUrl { get; set; }

        [DisplayName(nameof(TelegramUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string TelegramUrl { get; set; }

        [DisplayName(nameof(YoutubeUrl))]
        [DataType(DataType.Url)]
        [Url]
        public string YoutubeUrl { get; set; }

        [DisplayName(nameof(ShowPayment))]
        public bool ShowPayment { get; set; }

        [DisplayName(nameof(ShowInvite))]
        public bool ShowInvite { get; set; }

        public AppAboutLangModel AppAboutLang { get; set; }
    }

    public class AppAboutLangModel
    {
        [DisplayName($"{nameof(AboutCompany)}{PropertyAttributeConstants.EnLang}")]
        [DataType(DataType.MultilineText)]
        public string AboutCompany { get; set; }

        [DisplayName($"{nameof(AboutApp)}{PropertyAttributeConstants.EnLang}")]
        [DataType(DataType.MultilineText)]
        public string AboutApp { get; set; }

        [DisplayName($"{nameof(TermsAndConditions)}{PropertyAttributeConstants.EnLang}")]
        [DataType(DataType.MultilineText)]
        public string TermsAndConditions { get; set; }

        [DisplayName($"{nameof(QuestionsAndAnswer)}{PropertyAttributeConstants.EnLang}")]
        [DataType(DataType.MultilineText)]
        public string QuestionsAndAnswer { get; set; }

        [DisplayName($"{nameof(GameRules)}{PropertyAttributeConstants.EnLang}")]
        [DataType(DataType.MultilineText)]
        public string GameRules { get; set; }

        [DisplayName($"{nameof(Subscriptions)}{PropertyAttributeConstants.EnLang}")]
        [DataType(DataType.MultilineText)]
        public string Subscriptions { get; set; }

        [DisplayName($"{nameof(Prizes)}{PropertyAttributeConstants.EnLang}")]
        [DataType(DataType.MultilineText)]
        public string Prizes { get; set; }
    }
}
