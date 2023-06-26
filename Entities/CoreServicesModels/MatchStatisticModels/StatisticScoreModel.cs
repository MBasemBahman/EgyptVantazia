using Entities.DBModels.MatchStatisticModels;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.CoreServicesModels.MatchStatisticModels
{
    public class StatisticScoreParameters : RequestParameters
    {
        [DisplayName(nameof(StatisticCategory))]
        public int Fk_StatisticCategory { get; set; }
    }
    public class StatisticScoreModel : AuditLookUpEntity
    {
        [DisplayName(nameof(_365_Id))]
        public string _365_Id { get; set; }

        [DisplayName(nameof(StatisticCategory))]
        [ForeignKey(nameof(StatisticCategory))]
        public int Fk_StatisticCategory { get; set; }

        [DisplayName(nameof(StatisticCategory))]
        public StatisticCategoryModel StatisticCategory { get; set; }
    }

    public class StatisticScoreModelForCalc
    {
        public int Id { get; set; }
        public string _365_Id { get; set; }
        public string Name { get; set; }
    }

    public class StatisticScoreCreateOrEditModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public  string Name { get; set; }

        [DisplayName(nameof(_365_Id))]
        public string _365_Id { get; set; }

        [DisplayName(nameof(StatisticCategory))]
        public int Fk_StatisticCategory { get; set; }

        public StatisticScoreLangModel StatisticScoreLang { get; set; }
    }

    public class StatisticScoreLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
