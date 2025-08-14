import { Routes } from '@angular/router';
import { loginGuard } from './guards/login.guard';
import { roleGuard } from './guards/role.guard';

import { Login } from './components/login/login';
import { Register } from './components/register/register';
import { AdminDashboard } from './components/admin-dashboard/admin-dashboard';
import { UserDashboard } from './components/user-dashboard/user-dashboard';

export const routes: Routes = [
  {
    path: 'login',
    component: Login,
    canActivate: [loginGuard],
  },
  {
    path: 'register',
    component: Register,
    canActivate: [loginGuard],
  },
  {
    path: 'admin',
    component: AdminDashboard,
    canActivate: [roleGuard],
    data: { role: 'Admin' },
  },
  {
    path: 'user',
    component: UserDashboard,
    canActivate: [roleGuard],
    data: { role: 'User' },
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];
