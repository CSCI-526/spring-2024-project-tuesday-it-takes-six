import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv('new-raw-analytics.csv')
df['Timestamp'] = pd.to_datetime(df['Timestamp'])

# Filter for level completed events with deaths split by level. 
completed_levels = df[df['eventType'] == 'levelCompleted'][['sessionID', 'currentLevel']]
died_events = df[df['eventType'] == 'playerDied']
died_in_completed_levels = pd.merge(died_events, completed_levels, on=['sessionID', 'currentLevel'])
died_in_completed_levels = died_in_completed_levels.sort_values(by='Timestamp')

# Group deaths that occur within 2 seconds of each other as a single event
died_in_completed_levels['GroupedDeath'] = (died_in_completed_levels['Timestamp'].diff() > pd.Timedelta(seconds=2)).cumsum()
deaths_per_session_per_level = died_in_completed_levels.groupby(['currentLevel', 'sessionID', 'GroupedDeath']).size().groupby(['currentLevel', 'sessionID']).size()
deaths_per_session_per_level_df = deaths_per_session_per_level.reset_index(name='DeathCount')

players_completed_per_level = completed_levels.groupby('currentLevel')['sessionID'].nunique()

# Plotting
fig, ax = plt.subplots(figsize=(12, 8))
deaths_per_session_per_level_df.boxplot(column='DeathCount', by='currentLevel', ax=ax, showmeans=True)
plt.title('Number of Deaths per Player per Level (Only for Completed Levels)')
plt.suptitle('')  # Suppress the automatic title to only show the above title
plt.xlabel('Level')
plt.ylabel('Number of Deaths')
plt.xticks(rotation=0)
plt.grid(True, linestyle='--', alpha=0.7)

# Annotate the number of players that completed each level
for i, n in enumerate(players_completed_per_level):
    ax.text(i+1, ax.get_ylim()[1], f'Number Complete: {n}', horizontalalignment='center', verticalalignment='top')

plt.show()
