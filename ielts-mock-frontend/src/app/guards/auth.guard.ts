import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const AuthGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const isLoggedInSignal = auth.isLoggedIn;
  const isLoggedIn = typeof isLoggedInSignal === 'function' ? isLoggedInSignal() : !!isLoggedInSignal;

  if (isLoggedIn) {
    return true;
  }

  router.navigate(['/login']);
  return false;
};
