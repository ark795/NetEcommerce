﻿namespace OrderService.API.Domain.ValueObjects;
public class Address
{
    public string Province { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
}