import csv
import os
import shutil

SCRIPT_DIR = os.path.dirname(os.path.realpath(__file__))
GENERATED_DIR = os.path.join(os.path.dirname(SCRIPT_DIR), 'generated')

SCHEDULING_TABLE_PATH = os.path.join(GENERATED_DIR, 'scheduling.csv')
HACKS_TABLE_PATH = os.path.join(GENERATED_DIR, 'hacks.csv')
RUNNER_FLAGS_TABLE_PATH = os.path.join(GENERATED_DIR, 'runners.csv')

GENERATED_INFOS_PATH = os.path.join(GENERATED_DIR, 'infos')

SCHEDULING_COLUMN_RUN_HACK_ID = 0
SCHEDULING_COLUMN_RUN_HACK_NAME = 1
SCHEDULING_COLUMN_RUN_CATEGORY = 2
SCHEDULING_COLUMN_RUN_RUNNER_IDS = 3
SCHEDULING_COLUMN_RUN_NOTES = 4
SCHEDULING_COLUMN_RUN_HOURS = 5
SCHEDULING_COLUMN_RUN_MINUTES = 6

SCHEDULING_COLUMN_RUNNER_ID = 8
SCHEDULING_COLUMN_RUNNER_DISCORD = 9
SCHEDULING_COLUMN_RUNNER_NAME = 10

INFO_FMT_CODENAME = "codename = {codename}\n"
INFO_FMT_HACKNAME = "hackname = {hackname}\n"
INFO_FMT_CREATOR = "creator = {creator}\n"
INFO_FMT_RUNNER = "runner{num} = {name}\n"
INFO_FMT_FLAG = "flag{num} = {flag}\n"
INFO_FMT_CATEGORY = "category = {category}\n"
INFO_FMT_ESTIMATE = "estimate = {estimate}\n"
INFO_FMT_SCROLL = "scroll = {scroll}\n"
INFO_FMT_BARS = "bars = {bars}\n"
INFO_FMT_LAYOUT = "layout = {layout}\n"

def load_table(path):
    arr = []
    with open(path) as csvfile:
        reader = csv.reader(csvfile, delimiter='\t')
        first = True
        for row in reader:
            if first:
                first = False
            else:
                arr.append(row)

    return arr

class Run:
    def __init__(self, id, name, category, runners, hours, minutes):
        self.hack_id = id
        self.hack_name = name
        self.category = category
        self.hours = "" if hours == '' or hours == '?' or int(hours) == 0 else hours
        self.minutes = minutes
        self.runner_ids = [ id.strip() for id in runners.split(',') ]

    @property
    def estimate(self):
        return f"{self.hours}:{self.minutes:02}:00" if self.hours else f"{self.minutes}:00"

    def __str__(self):
        return f"Run(hack={self.hack_id}, name='{self.hack_name}', category='{self.category}', runners='{self.runner_ids}', estimate={self.estimate}"
    def __repr__(self):
        return self.__str__()

class Runner:
    def __init__(self, id, discord, name, runner_id_flags):
        self.id = id
        self.name = name if name else discord
        self.flag = runner_id_flags[id]
 
    def __str__(self):
        return f"Runner(id={self.id}, name='{self.name}', flag={self.flag})"
    def __repr__(self):
        return self.__str__()

class Hack:
    def __init__(self, codename, id, creator, bars, layout):
        self.codename = codename
        self.id = id
        self.creator = creator
        self.bars = bars
        self.layout = layout

    def __str__(self):
        return f"Hack(id={self.id}, codename={self.codename}, creator='{self.creator}', bars={self.bars})"
    def __repr__(self):
        return self.__str__()

if __name__ == '__main__':
    scheduling = load_table(SCHEDULING_TABLE_PATH)
    runs = [ Run(entry[SCHEDULING_COLUMN_RUN_HACK_ID]
               , entry[SCHEDULING_COLUMN_RUN_HACK_NAME]
               , entry[SCHEDULING_COLUMN_RUN_CATEGORY]
               , entry[SCHEDULING_COLUMN_RUN_RUNNER_IDS]
               , entry[SCHEDULING_COLUMN_RUN_HOURS]
               , entry[SCHEDULING_COLUMN_RUN_MINUTES]) for entry in scheduling if entry[SCHEDULING_COLUMN_RUN_HACK_ID] ]    

    runner_id_flags = { entry[0] : entry[1] for entry in load_table(RUNNER_FLAGS_TABLE_PATH) }
    runners = { entry[SCHEDULING_COLUMN_RUNNER_ID] : Runner(entry[SCHEDULING_COLUMN_RUNNER_ID]
                                                   , entry[SCHEDULING_COLUMN_RUNNER_DISCORD]
                                                   , entry[SCHEDULING_COLUMN_RUNNER_NAME]
                                                   , runner_id_flags) for entry in scheduling if entry[SCHEDULING_COLUMN_RUNNER_ID] }
    runners['TBD'] = Runner('to be determined', 'to be determined', 'to be determined', runner_id_flags)
    runners['???'] = Runner('to be determined', 'to be determined', 'to be determined', runner_id_flags)
    runners['host AA/AGL'] = Runner('AGL', 'aglab2', 'aglab2', runner_id_flags)
    runners['Lots of Runners'] = Runner('Lots of Runners', 'Lots of Runners', 'Lots of Runners', runner_id_flags)
    runners['contestants TBD'] = Runner('Lots of Runners', 'Lots of Runners', 'Lots of Runners', runner_id_flags)

    hacks = { entry[1] : Hack(entry[0], entry[1], entry[2], entry[3], entry[4]) for entry in load_table(HACKS_TABLE_PATH) }

    shutil.rmtree(GENERATED_INFOS_PATH, ignore_errors=True)
    os.makedirs(GENERATED_INFOS_PATH, exist_ok=True)

    for run in runs:
        hack = hacks[run.hack_id]
        info_name = f"{hack.codename} - {run.hack_name} - {run.category}.txt".replace('?', '').replace(':', '')
        info_path = os.path.join(GENERATED_INFOS_PATH, info_name)

        with open(info_path, 'w') as info_file:
            info_file.write(INFO_FMT_CODENAME.format(codename = hack.codename))
            info_file.write(INFO_FMT_HACKNAME.format(hackname = run.hack_name))
            info_file.write(INFO_FMT_CREATOR.format(creator = hack.creator))
            num = 0
            for runner_id in run.runner_ids:
                num = num + 1
                runner = runners[runner_id]
                info_file.write(INFO_FMT_RUNNER.format(num = num, name = runner.name))
                info_file.write(INFO_FMT_FLAG.format(num = num, flag = runner.flag))
            info_file.write(INFO_FMT_CATEGORY.format(category = run.category))
            info_file.write(INFO_FMT_ESTIMATE.format(estimate = run.estimate))
            info_file.write(INFO_FMT_SCROLL.format(scroll = ""))
            info_file.write(INFO_FMT_BARS.format(bars = hack.bars))
            if hack.layout:
                info_file.write(INFO_FMT_LAYOUT.format(layout = hack.layout))
