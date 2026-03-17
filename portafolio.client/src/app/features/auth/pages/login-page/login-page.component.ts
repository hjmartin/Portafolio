import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AuthFacade } from '../../../../core/auth/auth.facade';
import { SectionShellComponent } from '../../../../shared/components/section-shell/section-shell.component';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [CommonModule, FormsModule, SectionShellComponent],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css'
})
export class LoginPageComponent {
  private readonly authFacade = inject(AuthFacade);

  protected email = '';
  protected password = '';
  protected readonly isSubmitting = signal(false);
  protected readonly errorMessage = signal('');

  protected async submit(): Promise<void> {
    this.isSubmitting.set(true);
    this.errorMessage.set('');

    try {
      await this.authFacade.login({
        email: this.email.trim(),
        password: this.password
      });
    } catch (error) {
      if (error instanceof HttpErrorResponse) {
        if (error.status === 401) {
          this.errorMessage.set('Credenciales invalidas.');
        } else if (error.status === 0) {
          this.errorMessage.set('No fue posible conectar con el Gateway.');
        } else {
          this.errorMessage.set(`Error al iniciar sesion (${error.status}).`);
        }
      } else {
        this.errorMessage.set('No fue posible iniciar sesion.');
      }
    } finally {
      this.isSubmitting.set(false);
    }
  }
}
