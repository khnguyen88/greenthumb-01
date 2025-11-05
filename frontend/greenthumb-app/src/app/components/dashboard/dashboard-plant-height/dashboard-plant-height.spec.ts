import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardPlantHeight } from './dashboard-plant-height';

describe('DashboardPlantHeight', () => {
  let component: DashboardPlantHeight;
  let fixture: ComponentFixture<DashboardPlantHeight>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardPlantHeight]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardPlantHeight);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
