import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardSoilMoisture } from './dashboard-soil-moisture';

describe('DashboardSoilMoisture', () => {
  let component: DashboardSoilMoisture;
  let fixture: ComponentFixture<DashboardSoilMoisture>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardSoilMoisture]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardSoilMoisture);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
