import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardTemperature } from './dashboard-temperature';

describe('DashboardTemperature', () => {
  let component: DashboardTemperature;
  let fixture: ComponentFixture<DashboardTemperature>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardTemperature]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardTemperature);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
