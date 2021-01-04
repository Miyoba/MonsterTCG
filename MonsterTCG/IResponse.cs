using System;

namespace MonsterTCG
{
    public interface IResponse
    {
        String Content { get; set; }
        string ContentType { get; set; }
        StatusCodesEnum Status { get; set; }
    }
}