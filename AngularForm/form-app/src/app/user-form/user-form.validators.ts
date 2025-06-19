import { FormGroup } from '@angular/forms';

export class CustomValidators {
  static passwordMatchValidator(form: FormGroup): { [key: string]: any } | null {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  static blacklistUsernameValidator(blacklistedUsernames: string[]) {
    return (form: FormGroup): { [key: string]: any } | null => {
      const username: string = form.get('username')?.value?.toLowerCase() || '';
      for (let blacklistedName of blacklistedUsernames) {
        if (username.includes(blacklistedName)) {
          return { invalidName: true };
        }
      }
      return null;
    };
  }
}
