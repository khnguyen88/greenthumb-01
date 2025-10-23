export interface ChartData {
  labels: string[];
  datasets: ChartDataset[];
}

export interface ChartDataset {
  type: 'line' | 'bar' | null; // Extend with other chart types if needed
  label: string;
  data: number[];
  borderColor?: string;
  borderDash?: number[] | null;
  borderWidth?: number;
  backgroundColor?: string;
  fill?: boolean;
  tension?: number;
}

export interface ChartOptions {
  maintainAspectRatio: boolean;
  aspectRatio: number;
  plugins: {
    legend: {
      labels: {
        color: string;
      };
    };
  };
  scales: {
    x: AxisOptions;
    y: AxisOptions;
  };
}

export interface AxisOptions {
  ticks: {
    color: string;
  };
  grid: {
    color: string;
  };
}
