import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardHumidity } from './dashboard-humidity';

describe('DashboardHumidity', () => {
  let component: DashboardHumidity;
  let fixture: ComponentFixture<DashboardHumidity>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardHumidity]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardHumidity);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
