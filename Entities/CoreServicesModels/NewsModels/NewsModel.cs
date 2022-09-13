﻿using Entities.DBModels.NewsModels;
using Entities.DBModels.SeasonModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.EnumData.LogicEnumData;

namespace Entities.CoreServicesModels.NewsModels
{
    public class NewsParameters : RequestParameters
    {
        public int? Fk_GameWeak { get; set; }

    }
    public class NewsModel : AuditImageEntity
    {
        [DisplayName($"{nameof(Title)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Title { get; set; }

        [DisplayName($"{nameof(ShortDescription)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [DisplayName($"{nameof(LongDescription)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        [DataType(DataType.MultilineText)]
        public string LongDescription { get; set; }

        [DisplayName(nameof(NewsTypeEnum))]
        public NewsTypeEnum NewsTypeEnum { get; set; }

        [DisplayName(nameof(GameWeak))]
        [ForeignKey(nameof(GameWeak))]
        public int? Fk_GameWeak { get; set; }

    }
}
