select count(Name), TypeOfTea.ColorOfTea from TeaInfo join TypeOfTea on TypeOfTea.ID = TeaInfo.TypeId group by  TypeOfTea.ColorOfTea


