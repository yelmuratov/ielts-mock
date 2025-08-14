import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';

interface LoginCredentials {
  username: string;
  password: string;
}

interface RegisterCredentials {
  username: string;
  email: string;
  password: string;
}

interface LoginResponse {
  id: number;
  username: string;
  email: string;
  role: 'Admin' | 'User';
  token: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  private _isLoggedIn = signal<boolean>(this.hasToken());
  private _userRole = signal<string | null>(this.getStoredRole());

  get isLoggedIn() {
    return this._isLoggedIn;
  }

  get userRole() {
    return this._userRole;
  }

  // ✅ LOGIN
  login(credentials: LoginCredentials): Observable<LoginResponse> {
    return this.http.post<LoginResponse>('/api/auth/login', credentials).pipe(
      tap((res) => {
        if (this.isBrowser()) {
          localStorage.setItem('access_token', res.token);
          localStorage.setItem('user_role', res.role);
        }

        this._isLoggedIn.set(true);
        this._userRole.set(res.role);

        // Role-based redirect
        if (res.role === 'Admin') {
          this.router.navigate(['/admin']);
        } else {
          this.router.navigate(['/user']);
        }
      })
    );
  }

  // ✅ REGISTER
  register(credentials: RegisterCredentials): Observable<any> {
    return this.http.post('/api/auth/register', credentials);
  }

  // ✅ LOGOUT
  logout(): void {
    if (this.isBrowser()) {
      localStorage.removeItem('access_token');
      localStorage.removeItem('user_role');
    }

    this._isLoggedIn.set(false);
    this._userRole.set(null);
    this.router.navigate(['/login']);
  }

  // ✅ Token and Role helpers
  getToken(): string | null {
    return this.isBrowser() ? localStorage.getItem('access_token') : null;
  }

  getRole(): string | null {
    return this.userRole();
  }

  private hasToken(): boolean {
    return this.isBrowser() && !!localStorage.getItem('access_token');
  }

  private getStoredRole(): string | null {
    return this.isBrowser() ? localStorage.getItem('user_role') : null;
  }

  private isBrowser(): boolean {
    return typeof window !== 'undefined' && typeof localStorage !== 'undefined';
  }
}
