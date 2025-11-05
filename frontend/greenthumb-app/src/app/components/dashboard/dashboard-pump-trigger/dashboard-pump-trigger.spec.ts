import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardPumpTrigger } from './dashboard-pump-trigger';

describe('DashboardPumpTrigger', () => {
  let component: DashboardPumpTrigger;
  let fixture: ComponentFixture<DashboardPumpTrigger>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardPumpTrigger]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardPumpTrigger);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
