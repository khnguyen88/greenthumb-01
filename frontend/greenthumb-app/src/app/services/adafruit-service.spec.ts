import { TestBed } from '@angular/core/testing';

import { AdafruitService } from './adafruit-service';

describe('AdafruitService', () => {
  let service: AdafruitService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdafruitService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
