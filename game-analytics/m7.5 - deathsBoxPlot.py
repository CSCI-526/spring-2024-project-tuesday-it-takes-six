import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv('new-raw-analytics.csv')
df['Timestamp'] = pd.to_datetime(df['Timestamp'])

# Filter and sort only 'playerDied' events
died_events = df[df['eventType'] == 'playerDied']
died_events = died_events.sort_values(by='Timestamp')

# Deaths within 2 seconds of each other are a single event. 
died_events['GroupedDeath'] = (died_events['Timestamp'].diff() > pd.Timedelta(seconds=2)).cumsum()
deaths_per_session_per_level = died_events.groupby(['currentLevel', 'sessionID', 'GroupedDeath']).size().groupby(['currentLevel', 'sessionID']).size()
deaths_per_session_per_level_df = deaths_per_session_per_level.reset_index(name='DeathCount')

# Plotting
plt.figure(figsize=(12, 8))
deaths_per_session_per_level_df.boxplot(column='DeathCount', by='currentLevel', showmeans=True)
plt.title('Number of Deaths per Player per Level')
plt.suptitle('')  # Suppress the automatic title to only show the above title
plt.xlabel('Level')
plt.ylabel('Number of Deaths')
plt.xticks(rotation=0)
plt.grid(True, linestyle='--', alpha=0.7)
plt.show()
