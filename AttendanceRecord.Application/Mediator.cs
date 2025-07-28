using Mediator.Switch;

namespace AttendanceRecord.Application;

public class Mediator(ISwitchMediatorServiceProvider serviceProvider) : SwitchMediator(serviceProvider);
