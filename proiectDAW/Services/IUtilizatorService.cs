﻿using proiectDAW.Models;
using proiectDAW.Models.Authentication;
using proiectDAW.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace proiectDAW.Services
{
    public interface IUtilizatorService
    {
        UtilizatorDTO getUtilizatorByName(string nume, string prenume);
        UtilizatorDTO getUtilizatorByUsername(string username);
        UtilizatorDTO getUtilizatorByNameWithDate(string nume, string prenume);
        UtilizatorDTO createUtilizator(Utilizator utiliz);
        List<UtilizatorDTO> getAll();
        Utilizator FindById(Guid Id);
        UtilizatorDTO FindByIdWithData(Guid Id);
        UtilizatorDTO deleteUser(Utilizator utilizator);
        void Save();
        UtilizatorResponseDTO Authenticate(UtilizatorRequestDTO request);
    }
}
