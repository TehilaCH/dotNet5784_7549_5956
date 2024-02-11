

namespace BlImplementation;
using BlApi;
using BO;
using Dal;
using System;
using System.Xml.Linq;

internal class ScheduleImplementation : BlApi.ISchedule
{
    private readonly DalApi.IDal _dal = (DalApi.IDal)Factory.Get;
  
    public DateTime? getEndProjectDate() => _dal.Schedule.getEndProjectDate();//A function that returns a date to a file
    public DateTime? getStartProjectDate() => _dal.Schedule.getStartProjectDate();//A function that returns a date to a file

    public DateTime? SetStartProjectDate(DateTime date)=>_dal.Schedule.SetEndProjectDate(date);//A function that writes a date to a file

    public DateTime? SetEndProjectDate(DateTime date) => _dal.Schedule.SetStartProjectDate(date);//A function that writes a date to a file



}
