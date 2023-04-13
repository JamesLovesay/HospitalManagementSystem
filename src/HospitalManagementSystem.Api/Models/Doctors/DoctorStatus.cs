﻿namespace HospitalManagementSystem.Api.Models.Doctors
{
    public enum DoctorStatus
    {
        // Must be kept in alphabetical order to protect sort function in repository
        ActivePermanent,
        ActiveVisiting,
        Inactive
    }
}
