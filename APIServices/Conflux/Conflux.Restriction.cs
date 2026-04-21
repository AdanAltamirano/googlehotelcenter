using System;
using System.Collections.Generic;
using APIServices.Conflux.Parser.Restriction;
using APIServices.Conflux.Models.Restrictions.Rate;

namespace APIServices.Conflux
{
    public partial class ConfluxService
    {

        private ConfluxEntities confluxEntities = new ConfluxEntities();

        private void GetOccupationMessages(List<RateRestrictionDto> listRestrictionDto,DateTime? startDate,DateTime? endDate)
        {
            var availStatusMessages = RestrictionsParser.ToAvailStatusMessagesOccupation(listRestrictionDto, startDate, endDate);

        }

        private void GetAdvancedDaysMessages(List<RateRestrictionDto> listRestrictionDto, DateTime? startDate, DateTime? endDate)
        {
            //var availStatusMessages = RestrictionsParser.ToAvailStatusMessagesAdvancedDays(listRestrictionDto, startDate, endDate);

        }

    }
}
