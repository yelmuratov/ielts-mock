import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn = (route) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (!auth.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }

  const expectedRole = route.data?.['role'];
  const userRole = auth.getRole();

  if (expectedRole && expectedRole !== userRole) {
    // Optionally redirect unauthorized users
    router.navigate(['/unauthorized']);
    return false;
  }

  return true;
};
