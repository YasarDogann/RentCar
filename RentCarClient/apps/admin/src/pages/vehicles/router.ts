import { inject } from "@angular/core";
import { Routes } from "@angular/router";
import { Common } from "../../services/common";

const router: Routes = [
    {
        path: '',
        loadComponent: () => import('./vehicles'),
        canActivate: [() => inject(Common).checkPermissionForRoute('vehicle:view')]
    },
    {
        path: 'add',
        loadComponent: () => import('./create/create'),
        canActivate: [() => inject(Common).checkPermissionForRoute('vehicle:create')]
    },
    {
        path: 'edit/:id',
        loadComponent: () => import('./create/create'),
        canActivate: [() => inject(Common).checkPermissionForRoute('vehicle:edit')]
    },
];

export default router;