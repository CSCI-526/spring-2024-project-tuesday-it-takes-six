import pandas as pd
import matplotlib.pyplot as plt
from datetime import datetime, timedelta

df = pd.read_csv('new-raw-analytics.csv')
df['Timestamp'] = pd.to_datetime(df['Timestamp'])
died_events = df[df['eventType'] == 'playerDied']
died_events = died_events.sort_values(by='Timestamp')

# laser deaths are repeated in the data (unintentional consequence)
died_events['LaserGroup'] = (died_events['Timestamp'].diff() > timedelta(seconds=1)).cumsum()
death_counts = died_events.groupby(['LaserGroup']).count()
laser_deaths = death_counts[death_counts['Timestamp'] > 3].index
died_events['DeathType'] = died_events['LaserGroup'].apply(lambda x: 'died by laser' if x in laser_deaths else 'died by other causes')

# Other deaths
death_summary = died_events.groupby(['currentLevel', 'DeathType']).size().unstack(fill_value=0)
death_summary['TotalDeaths'] = death_summary.sum(axis=1)
death_summary['LaserPercent'] = (death_summary['died by laser'] / death_summary['TotalDeaths']) * 100
death_summary['OtherCausesPercent'] = (death_summary['died by other causes'] / death_summary['TotalDeaths']) * 100

# Plotting
for level in death_summary.index:
    plt.figure()
    death_summary.loc[level, ['LaserPercent', 'OtherCausesPercent']].plot(kind='bar')
    plt.title(f'Percentage of Death Types for Level {level}')
    plt.ylabel('Percentage')
    plt.xlabel('Death Type')
    plt.xticks(rotation=0)
    plt.show()
