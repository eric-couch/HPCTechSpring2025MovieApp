﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCTechSpring2025MovieApp.Shared.Wrapper;

public class DataResponse<T> : Response
{
    public T Data { get; set; }

    public DataResponse() : base()
    {
    }

    public DataResponse(T data)
    {
        Succeeded = true;
        Data = data;
    }
}
