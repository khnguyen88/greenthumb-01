import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardWaterLevel } from './dashboard-water-level';

describe('DashboardWaterLevel', () => {
  let component: DashboardWaterLevel;
  let fixture: ComponentFixture<DashboardWaterLevel>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardWaterLevel]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardWaterLevel);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
