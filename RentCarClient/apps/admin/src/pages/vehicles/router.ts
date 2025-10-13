import { inject } from "@angular/core";
import { Routes } from "@angular/router";
import { Common } from "../../services/common";

const router: Routes = [
    {
        path: '',
        loadComponent: () => import('./vehicles'),
        canActivate: [() => inject(Common).checkPermissionForRoute('vehicle:view')]
    },
    
];

export default router;