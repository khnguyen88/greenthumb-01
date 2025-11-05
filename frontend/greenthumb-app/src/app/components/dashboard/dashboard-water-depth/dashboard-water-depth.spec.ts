import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardWaterDepth } from './dashboard-water-depth';

describe('DashboardWaterDepth', () => {
  let component: DashboardWaterDepth;
  let fixture: ComponentFixture<DashboardWaterDepth>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardWaterDepth]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardWaterDepth);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
