import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { AuthCaGuard } from './auth-ca-guard';

describe('authCaGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => AuthCaGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
