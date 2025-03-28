﻿using Entities.DBModels.SeasonModels;

namespace Entities.DBModels.PrivateLeagueModels
{
    [Index(nameof(UniqueCode), IsUnique = true)]
    public class PrivateLeague : AuditEntity
    {
        [DisplayName($"{nameof(Name)}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(GameWeak))]
        [ForeignKey(nameof(GameWeak))]
        public int? Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeak GameWeak { get; set; }

        [DisplayName(nameof(UniqueCode))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string UniqueCode { get; set; }

        [DisplayName(nameof(PrivateLeagueMembers))]
        public IList<PrivateLeagueMember> PrivateLeagueMembers { get; set; }
    }
}
