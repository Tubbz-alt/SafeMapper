﻿namespace SafeMapper.Profiler
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using EmitMapper;

    using FastMapper;

    using SafeMapper;

    public class ProfileListToList : ProfileBase
    {
        public override void Execute()
        {
            var rand = new Random();
            var fromType = typeof(List<int>);
            var toType = typeof(List<decimal>);

            var intList = new List<int>(50);
            for (int i = 0; i < 50; i++)
            {
                intList.Add(rand.Next());
            }

            var toElementType = typeof(decimal);

            var fastConverter = SafeMap.GetConverter<List<int>, List<decimal>>();
            var emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<List<int>, List<decimal>>();

            this.WriteHeader();


            this.AddResult("Array.ConvertAll todecimal", i => intList.ConvertAll(Convert.ToDecimal));
            this.AddResult("Array.ConvertAll changetype", i => intList.ConvertAll(v => (decimal)Convert.ChangeType(v, toElementType)));
            this.AddResult("EmitMapper", i => emitMapper.Map(intList));
            this.AddResult("SafeMapper", i => fastConverter(intList));
            this.AddResult("FastMapper", i => TypeAdapter.Adapt(intList, fromType, toType));
            this.AddResult("AutoMapper", i => Mapper.Map(intList, fromType, toType));
        }
    }
}
