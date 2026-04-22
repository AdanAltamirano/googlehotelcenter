using System;
using System.Collections.Generic;
using APIServices.Conflux.Parser.Restriction;
using APIServices.Conflux.Models.Restrictions.Rate;

namespace APIServices.Conflux
{
    public partial class ConfluxService
    {

        private ConfluxEntities confluxEntities = new ConfluxEntities();

        private OTA.Models.Restrictions.AvailStatusMessages GetOccupationAvailStatusMessages(List<RateRestrictionDto> listRestrictionDto,DateTime? startDate,DateTime? endDate)
        {
            var availStatusMessages = RestrictionsParser.ToAvailStatusMessagesOccupation(listRestrictionDto, startDate, endDate);

            return availStatusMessages;

        }

        private void GetAdvancedDaysAvailStatusMessages(List<RateRestrictionDto> listRestrictionDto, DateTime? startDate, DateTime? endDate)
        {
            var availStatusMessages = RestrictionsParser.ToAvailStatusMessagesAdvancedDays(listRestrictionDto, startDate, endDate);

        }

    }
}
